import { Injectable, inject } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

export type Language = 'tr' | 'en';

@Injectable({ providedIn: 'root' })
export class LanguageService {
  private translate = inject(TranslateService);
  private readonly STORAGE_KEY = 'language';

  /**
   * Uygulama açılışında çağrılır — kullanıcının önceki tercihini geri yükler.
   * Tercih yoksa Türkçe ile başlar.
   */
  init(): void {
  const saved = localStorage.getItem(this.STORAGE_KEY) as Language | null;
  this.setLanguage(saved ?? 'tr');
}

  setLanguage(lang: Language): void {
    this.translate.use(lang);
    localStorage.setItem(this.STORAGE_KEY, lang);
  }

  getLanguage(): Language {
    return (localStorage.getItem(this.STORAGE_KEY) as Language) ?? 'tr';
  }

  toggle(): void {
    this.setLanguage(this.getLanguage() === 'tr' ? 'en' : 'tr');
  }
}