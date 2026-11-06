import { ConflictException, Injectable, NotFoundException } from '@nestjs/common';
import { Prisma } from '../generated/prisma/client.js';
import { PrismaService } from '../prisma/prisma.service.js';
import type { CreateFornecedorDto, UpdateFornecedorDto } from './fornecedores.schema.js';

@Injectable()
export class FornecedoresService {
  constructor(private readonly prisma: PrismaService) {}

  findAll() {
    return this.prisma.fornecedor.findMany();
  }

  async findOne(id: string) {
    const fornecedor = await this.prisma.fornecedor.findUnique({ where: { id } });
    if (!fornecedor) throw new NotFoundException('Fornecedor não encontrado');
    return fornecedor;
  }

  async create(data: CreateFornecedorDto) {
    try {
      return await this.prisma.fornecedor.create({ data });
    } catch (error) {
      if (error instanceof Prisma.PrismaClientKnownRequestError && error.code === 'P2002') {
        throw new ConflictException('Já existe um fornecedor com esse CNPJ');
      }
      throw error;
    }
  }

  async update(id: string, data: UpdateFornecedorDto) {
    await this.findOne(id);
    try {
      return await this.prisma.fornecedor.update({ where: { id }, data });
    } catch (error) {
      if (error instanceof Prisma.PrismaClientKnownRequestError && error.code === 'P2002') {
        throw new ConflictException('Já existe um fornecedor com esse CNPJ');
      }
      throw error;
    }
  }

  async remove(id: string) {
    await this.findOne(id);
    await this.prisma.fornecedor.delete({ where: { id } });
  }
}
