import { BadRequestException, NotFoundException } from '@nestjs/common';
import { MovimentacoesService } from './movimentacoes.service.js';

function createTx() {
  return {
    produto: { findUnique: vi.fn(), update: vi.fn() },
    movimentacaoEstoque: { create: vi.fn() },
  };
}

function createPrismaMock() {
  const tx = createTx();
  return {
    tx,
    $transaction: vi.fn((callback: (tx: ReturnType<typeof createTx>) => unknown) => callback(tx)),
  };
}

describe('MovimentacoesService', () => {
  it('rejects SAIDA larger than current stock', async () => {
    const prisma = createPrismaMock();
    prisma.tx.produto.findUnique.mockResolvedValue({ id: 'p1', quantidade: 2 });
    const service = new MovimentacoesService(prisma as never);

    await expect(
      service.create({ produtoId: 'p1', tipo: 'SAIDA', quantidade: 5 }),
    ).rejects.toThrow(BadRequestException);
    expect(prisma.tx.produto.update).not.toHaveBeenCalled();
  });

  it('throws NotFoundException when produto does not exist', async () => {
    const prisma = createPrismaMock();
    prisma.tx.produto.findUnique.mockResolvedValue(null);
    const service = new MovimentacoesService(prisma as never);

    await expect(
      service.create({ produtoId: 'missing', tipo: 'ENTRADA', quantidade: 1 }),
    ).rejects.toThrow(NotFoundException);
  });

  it('increments produto quantity on ENTRADA and registers the movement', async () => {
    const prisma = createPrismaMock();
    prisma.tx.produto.findUnique.mockResolvedValue({ id: 'p1', quantidade: 10 });
    prisma.tx.movimentacaoEstoque.create.mockResolvedValue({ id: 'm1' });
    const service = new MovimentacoesService(prisma as never);

    const result = await service.create({ produtoId: 'p1', tipo: 'ENTRADA', quantidade: 3 });

    expect(prisma.tx.produto.update).toHaveBeenCalledWith({
      where: { id: 'p1' },
      data: { quantidade: { increment: 3 } },
    });
    expect(result).toEqual({ id: 'm1' });
  });

  it('decrements produto quantity on SAIDA within stock limits', async () => {
    const prisma = createPrismaMock();
    prisma.tx.produto.findUnique.mockResolvedValue({ id: 'p1', quantidade: 10 });
    prisma.tx.movimentacaoEstoque.create.mockResolvedValue({ id: 'm2' });
    const service = new MovimentacoesService(prisma as never);

    await service.create({ produtoId: 'p1', tipo: 'SAIDA', quantidade: 4 });

    expect(prisma.tx.produto.update).toHaveBeenCalledWith({
      where: { id: 'p1' },
      data: { quantidade: { increment: -4 } },
    });
  });
});
