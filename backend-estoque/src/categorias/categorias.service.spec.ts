import { ConflictException, NotFoundException } from '@nestjs/common';
import { Prisma } from '../generated/prisma/client.js';
import { CategoriasService } from './categorias.service.js';

function createPrismaMock() {
  return {
    categoria: {
      findMany: vi.fn(),
      findUnique: vi.fn(),
      create: vi.fn(),
      update: vi.fn(),
      delete: vi.fn(),
    },
  };
}

describe('CategoriasService', () => {
  let prisma: ReturnType<typeof createPrismaMock>;
  let service: CategoriasService;

  beforeEach(() => {
    prisma = createPrismaMock();
    service = new CategoriasService(prisma as never);
  });

  it('throws NotFoundException when category does not exist', async () => {
    prisma.categoria.findUnique.mockResolvedValue(null);
    await expect(service.findOne('missing-id')).rejects.toThrow(NotFoundException);
  });

  it('throws ConflictException when name is duplicated', async () => {
    prisma.categoria.create.mockRejectedValue(
      new Prisma.PrismaClientKnownRequestError('duplicate', { code: 'P2002', clientVersion: '7.10.1' }),
    );
    await expect(service.create({ nome: 'Eletrônicos' })).rejects.toThrow(ConflictException);
  });
});
