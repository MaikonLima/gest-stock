import { z } from 'zod';

export const createCategoriaSchema = z.object({
  nome: z.string().min(1).max(100),
});
export type CreateCategoriaDto = z.infer<typeof createCategoriaSchema>;

export const updateCategoriaSchema = createCategoriaSchema.partial();
export type UpdateCategoriaDto = z.infer<typeof updateCategoriaSchema>;
