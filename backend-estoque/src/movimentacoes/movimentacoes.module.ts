import { Module } from '@nestjs/common';
import { MovimentacoesController } from './movimentacoes.controller.js';
import { MovimentacoesService } from './movimentacoes.service.js';

@Module({
  controllers: [MovimentacoesController],
  providers: [MovimentacoesService],
})
export class MovimentacoesModule {}
