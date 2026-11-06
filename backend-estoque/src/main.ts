import 'dotenv/config';
import { StandardSchemaValidationPipe } from '@nestjs/common';
import { NestFactory } from '@nestjs/core';
import { DocumentBuilder, SwaggerModule, type SwaggerDocumentOptions } from '@nestjs/swagger';
import { createSchema } from 'zod-openapi';
import { AppModule, ObserveInstrument } from './app.module.js';

async function bootstrap() {
  const app = await NestFactory.create(AppModule, {
    instrument: ObserveInstrument,
  });
  app.useGlobalPipes(new StandardSchemaValidationPipe());

  const swaggerConfig = new DocumentBuilder()
    .setTitle('Gerenciamento de Estoque')
    .setDescription('API para gestão de categorias, fornecedores, produtos e movimentações de estoque')
    .setVersion('1.0')
    .build();
  const swaggerOptions: SwaggerDocumentOptions = {
    standardSchemaConverter: (schema, { schemaType }) => {
      const converted = createSchema(schema as never, { io: schemaType, openapiVersion: '3.0.0' });
      return { schema: converted.schema, components: converted.components };
    },
  };
  const swaggerDocument = SwaggerModule.createDocument(app, swaggerConfig, swaggerOptions);
  SwaggerModule.setup('docs', app, swaggerDocument);

  await app.listen(process.env.PORT ?? 3000);
}
await bootstrap();
