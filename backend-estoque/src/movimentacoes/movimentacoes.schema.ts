import { z } from 'zod';

export const createMovimentacaoSchema = z.object({
  produtoId: z.uuid(),
  tipo: z.enum(['ENTRADA', 'SAIDA']),
  quantidade: z.number().int().positive(),
  motivo: z.string().max(500).optional(),
});
export type CreateMovimentacaoDto = z.infer<typeof createMovimentacaoSchema>;
