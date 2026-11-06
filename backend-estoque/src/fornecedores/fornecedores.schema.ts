import { z } from 'zod';

export const createFornecedorSchema = z.object({
  nome: z.string().min(1).max(150),
  cnpj: z.string().min(1).max(20).optional(),
  telefone: z.string().min(1).max(20).optional(),
  email: z.email().optional(),
});
export type CreateFornecedorDto = z.infer<typeof createFornecedorSchema>;

export const updateFornecedorSchema = createFornecedorSchema.partial();
export type UpdateFornecedorDto = z.infer<typeof updateFornecedorSchema>;
