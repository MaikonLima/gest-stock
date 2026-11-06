import { BadRequestException, ConflictException, Injectable, NotFoundException } from '@nestjs/common';
import { Prisma } from '../generated/prisma/client.js';
import { PrismaService } from '../prisma/prisma.service.js';
import type { CreateProdutoDto, UpdateProdutoDto } from './produtos.schema.js';

@Injectable()
export class ProdutosService {
  constructor(private readonly prisma: PrismaService) {}

  findAll() {
    return this.prisma.produto.findMany();
  }

  async findOne(id: string) {
    const produto = await this.prisma.produto.findUnique({ where: { id } });
    if (!produto) throw new NotFoundException('Produto não encontrado');
    return produto;
  }

  async create(data: CreateProdutoDto) {
    try {
      return await this.prisma.produto.create({ data });
    } catch (error) {
      throw this.mapPrismaError(error);
    }
  }

  async update(id: string, data: UpdateProdutoDto) {
    await this.findOne(id);
    try {
      return await this.prisma.produto.update({ where: { id }, data });
    } catch (error) {
      throw this.mapPrismaError(error);
    }
  }

  async remove(id: string) {
    await this.findOne(id);
    await this.prisma.produto.delete({ where: { id } });
  }

  private mapPrismaError(error: unknown) {
    if (error instanceof Prisma.PrismaClientKnownRequestError) {
      if (error.code === 'P2002') return new ConflictException('Já existe um produto com esse SKU');
      if (error.code === 'P2003') return new BadRequestException('Categoria ou fornecedor informado não existe');
    }
    return error;
  }
}
