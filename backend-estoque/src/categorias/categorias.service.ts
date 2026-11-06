import { ConflictException, Injectable, NotFoundException } from '@nestjs/common';
import { Prisma } from '../generated/prisma/client.js';
import { PrismaService } from '../prisma/prisma.service.js';
import type { CreateCategoriaDto, UpdateCategoriaDto } from './categorias.schema.js';

@Injectable()
export class CategoriasService {
  constructor(private readonly prisma: PrismaService) {}

  findAll() {
    return this.prisma.categoria.findMany();
  }

  async findOne(id: string) {
    const categoria = await this.prisma.categoria.findUnique({ where: { id } });
    if (!categoria) throw new NotFoundException('Categoria não encontrada');
    return categoria;
  }

  async create(data: CreateCategoriaDto) {
    try {
      return await this.prisma.categoria.create({ data });
    } catch (error) {
      if (error instanceof Prisma.PrismaClientKnownRequestError && error.code === 'P2002') {
        throw new ConflictException('Já existe uma categoria com esse nome');
      }
      throw error;
    }
  }

  async update(id: string, data: UpdateCategoriaDto) {
    await this.findOne(id);
    try {
      return await this.prisma.categoria.update({ where: { id }, data });
    } catch (error) {
      if (error instanceof Prisma.PrismaClientKnownRequestError && error.code === 'P2002') {
        throw new ConflictException('Já existe uma categoria com esse nome');
      }
      throw error;
    }
  }

  async remove(id: string) {
    await this.findOne(id);
    await this.prisma.categoria.delete({ where: { id } });
  }
}
