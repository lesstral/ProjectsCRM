export function stripTime(value: string | undefined): string {
  if (!value) return '';
  return value.split('T')[0] as string;
}