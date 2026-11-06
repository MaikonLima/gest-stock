import { z } from 'zod';

export const createProdutoSchema = z.object({
  nome: z.string().min(1).max(150),
  sku: z.string().min(1).max(50),
  descricao: z.string().max(1000).optional(),
  preco: z.number().positive(),
  quantidade: z.number().int().nonnegative().default(0),
  categoriaId: z.uuid().optional(),
  fornecedorId: z.uuid().optional(),
});
export type CreateProdutoDto = z.infer<typeof createProdutoSchema>;

export const updateProdutoSchema = createProdutoSchema.omit({ quantidade: true }).partial();
export type UpdateProdutoDto = z.infer<typeof updateProdutoSchema>;
