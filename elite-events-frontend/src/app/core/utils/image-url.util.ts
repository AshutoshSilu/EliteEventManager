import { environment } from '@environments/environment';

const API_ORIGIN = (() => {
  try {
    return new URL(environment.apiUrl).origin;
  } catch {
    return '';
  }
})();

const ABSOLUTE_URL_PATTERN = /^(?:[a-z][a-z0-9+\-.]*:)?\/\//i;

export function resolveImageUrl(url: string | null | undefined, fallback: string): string {
  if (!url || !url.trim()) return fallback;

  const normalized = url.trim().replace(/\\/g, '/');

  if (/^(data:|blob:)/i.test(normalized)) return normalized;
  if (ABSOLUTE_URL_PATTERN.test(normalized)) return normalized;

  if (!API_ORIGIN) {
    return normalized.startsWith('/') ? normalized : `/${normalized}`;
  }

  return normalized.startsWith('/') ? `${API_ORIGIN}${normalized}` : `${API_ORIGIN}/${normalized}`;
}