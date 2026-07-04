import { Injectable, signal } from '@angular/core';
import { APP_CONSTANTS } from '../constants/app-constants';

export type Theme = 'light' | 'dark';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private currentTheme = signal<Theme>(this.loadTheme());
  readonly theme = this.currentTheme.asReadonly();

  constructor() {
    this.applyTheme(this.currentTheme());
  }

  toggleTheme(): void {
    const newTheme: Theme = this.currentTheme() === 'light' ? 'dark' : 'light';
    this.setTheme(newTheme);
  }

  setTheme(theme: Theme): void {
    this.currentTheme.set(theme);
    localStorage.setItem(APP_CONSTANTS.themeKey, theme);
    this.applyTheme(theme);
  }

  private applyTheme(theme: Theme): void {
    document.body.setAttribute('data-theme', theme);
    document.body.classList.toggle('dark-theme', theme === 'dark');
  }

  private loadTheme(): Theme {
    const saved = localStorage.getItem(APP_CONSTANTS.themeKey) as Theme;
    if (saved) return saved;
    return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
  }
}
