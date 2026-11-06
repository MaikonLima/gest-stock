import { Body, Controller, Delete, Get, HttpCode, HttpStatus, Param, Patch, Post } from '@nestjs/common';
import { idParamSchema } from '../common/id-param.schema.js';
import { FornecedoresService } from './fornecedores.service.js';
import { createFornecedorSchema, updateFornecedorSchema } from './fornecedores.schema.js';
import type { CreateFornecedorDto, UpdateFornecedorDto } from './fornecedores.schema.js';

@Controller('fornecedores')
export class FornecedoresController {
  constructor(private readonly fornecedoresService: FornecedoresService) {}

  @Get()
  findAll() {
    return this.fornecedoresService.findAll();
  }

  @Get(':id')
  findOne(@Param({ schema: idParamSchema }) { id }: { id: string }) {
    return this.fornecedoresService.findOne(id);
  }

  @Post()
  create(@Body({ schema: createFornecedorSchema }) dto: CreateFornecedorDto) {
    return this.fornecedoresService.create(dto);
  }

  @Patch(':id')
  update(
    @Param({ schema: idParamSchema }) { id }: { id: string },
    @Body({ schema: updateFornecedorSchema }) dto: UpdateFornecedorDto,
  ) {
    return this.fornecedoresService.update(id, dto);
  }

  @Delete(':id')
  @HttpCode(HttpStatus.NO_CONTENT)
  remove(@Param({ schema: idParamSchema }) { id }: { id: string }) {
    return this.fornecedoresService.remove(id);
  }
}
