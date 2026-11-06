import { BadRequestException, ConflictException } from '@nestjs/common';
import { Prisma } from '../generated/prisma/client.js';
import { ProdutosService } from './produtos.service.js';

function createPrismaMock() {
  return {
    produto: { findMany: vi.fn(), findUnique: vi.fn(), create: vi.fn(), update: vi.fn(), delete: vi.fn() },
  };
}

describe('ProdutosService', () => {
  it('throws ConflictException when sku is duplicated', async () => {
    const prisma = createPrismaMock();
    prisma.produto.create.mockRejectedValue(
      new Prisma.PrismaClientKnownRequestError('duplicate', { code: 'P2002', clientVersion: '7.10.1' }),
    );
    const service = new ProdutosService(prisma as never);

    await expect(
      service.create({ nome: 'Mouse', sku: 'SKU-1', preco: 10, quantidade: 0 }),
    ).rejects.toThrow(ConflictException);
  });

  it('throws BadRequestException when categoriaId/fornecedorId does not exist', async () => {
    const prisma = createPrismaMock();
    prisma.produto.create.mockRejectedValue(
      new Prisma.PrismaClientKnownRequestError('fk violation', { code: 'P2003', clientVersion: '7.10.1' }),
    );
    const service = new ProdutosService(prisma as never);

    await expect(
      service.create({ nome: 'Mouse', sku: 'SKU-1', preco: 10, quantidade: 0, categoriaId: 'bad-id' }),
    ).rejects.toThrow(BadRequestException);
  });
});
