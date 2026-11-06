import { Body, Controller, Delete, Get, HttpCode, HttpStatus, Param, Patch, Post } from '@nestjs/common';
import { idParamSchema } from '../common/id-param.schema.js';
import { ProdutosService } from './produtos.service.js';
import { createProdutoSchema, updateProdutoSchema } from './produtos.schema.js';
import type { CreateProdutoDto, UpdateProdutoDto } from './produtos.schema.js';

@Controller('produtos')
export class ProdutosController {
  constructor(private readonly produtosService: ProdutosService) {}

  @Get()
  findAll() {
    return this.produtosService.findAll();
  }

  @Get(':id')
  findOne(@Param({ schema: idParamSchema }) { id }: { id: string }) {
    return this.produtosService.findOne(id);
  }

  @Post()
  create(@Body({ schema: createProdutoSchema }) dto: CreateProdutoDto) {
    return this.produtosService.create(dto);
  }

  @Patch(':id')
  update(
    @Param({ schema: idParamSchema }) { id }: { id: string },
    @Body({ schema: updateProdutoSchema }) dto: UpdateProdutoDto,
  ) {
    return this.produtosService.update(id, dto);
  }

  @Delete(':id')
  @HttpCode(HttpStatus.NO_CONTENT)
  remove(@Param({ schema: idParamSchema }) { id }: { id: string }) {
    return this.produtosService.remove(id);
  }
}
