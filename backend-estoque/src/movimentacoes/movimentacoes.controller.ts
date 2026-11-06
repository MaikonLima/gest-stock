import { Body, Controller, Get, HttpCode, HttpStatus, Param, Post } from '@nestjs/common';
import { idParamSchema } from '../common/id-param.schema.js';
import { MovimentacoesService } from './movimentacoes.service.js';
import { createMovimentacaoSchema } from './movimentacoes.schema.js';
import type { CreateMovimentacaoDto } from './movimentacoes.schema.js';

@Controller('movimentacoes')
export class MovimentacoesController {
  constructor(private readonly movimentacoesService: MovimentacoesService) {}

  @Get()
  findAll() {
    return this.movimentacoesService.findAll();
  }

  @Get(':id')
  findOne(@Param({ schema: idParamSchema }) { id }: { id: string }) {
    return this.movimentacoesService.findOne(id);
  }

  @Post()
  @HttpCode(HttpStatus.CREATED)
  create(@Body({ schema: createMovimentacaoSchema }) dto: CreateMovimentacaoDto) {
    return this.movimentacoesService.create(dto);
  }
}
