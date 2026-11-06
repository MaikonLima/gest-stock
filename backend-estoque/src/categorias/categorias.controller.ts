import { Body, Controller, Delete, Get, HttpCode, HttpStatus, Param, Patch, Post } from '@nestjs/common';
import { idParamSchema } from '../common/id-param.schema.js';
import { CategoriasService } from './categorias.service.js';
import { createCategoriaSchema, updateCategoriaSchema } from './categorias.schema.js';
import type { CreateCategoriaDto, UpdateCategoriaDto } from './categorias.schema.js';

@Controller('categorias')
export class CategoriasController {
  constructor(private readonly categoriasService: CategoriasService) {}

  @Get()
  findAll() {
    return this.categoriasService.findAll();
  }

  @Get(':id')
  findOne(@Param({ schema: idParamSchema }) { id }: { id: string }) {
    return this.categoriasService.findOne(id);
  }

  @Post()
  create(@Body({ schema: createCategoriaSchema }) dto: CreateCategoriaDto) {
    return this.categoriasService.create(dto);
  }

  @Patch(':id')
  update(
    @Param({ schema: idParamSchema }) { id }: { id: string },
    @Body({ schema: updateCategoriaSchema }) dto: UpdateCategoriaDto,
  ) {
    return this.categoriasService.update(id, dto);
  }

  @Delete(':id')
  @HttpCode(HttpStatus.NO_CONTENT)
  remove(@Param({ schema: idParamSchema }) { id }: { id: string }) {
    return this.categoriasService.remove(id);
  }
}
