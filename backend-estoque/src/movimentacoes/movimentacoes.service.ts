import { BadRequestException, Injectable, NotFoundException } from '@nestjs/common';
import { PrismaService } from '../prisma/prisma.service.js';
import type { CreateMovimentacaoDto } from './movimentacoes.schema.js';

@Injectable()
export class MovimentacoesService {
  constructor(private readonly prisma: PrismaService) {}

  findAll() {
    return this.prisma.movimentacaoEstoque.findMany({ orderBy: { createdAt: 'desc' } });
  }

  async findOne(id: string) {
    const movimentacao = await this.prisma.movimentacaoEstoque.findUnique({ where: { id } });
    if (!movimentacao) throw new NotFoundException('Movimentação não encontrada');
    return movimentacao;
  }

  create(data: CreateMovimentacaoDto) {
    return this.prisma.$transaction(async (tx) => {
      const produto = await tx.produto.findUnique({ where: { id: data.produtoId } });
      if (!produto) throw new NotFoundException('Produto não encontrado');

      if (data.tipo === 'SAIDA' && produto.quantidade < data.quantidade) {
        throw new BadRequestException('Estoque insuficiente para essa saída');
      }

      const delta = data.tipo === 'ENTRADA' ? data.quantidade : -data.quantidade;
      await tx.produto.update({
        where: { id: data.produtoId },
        data: { quantidade: { increment: delta } },
      });

      return tx.movimentacaoEstoque.create({ data });
    });
  }
}
