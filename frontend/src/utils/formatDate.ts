export function formatDate(dateString: string): string {
  if (!dateString) return 'Not set';
  return new Date(dateString).toLocaleDateString('ru-RU', {
    year: 'numeric',
    day: 'numeric',
    month: 'numeric',
  });
}