import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LanguageService, Language } from '../../core/services/language.service';

@Component({
  selector: 'app-language-switcher',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './language-switcher.html',
  styleUrl: './language-switcher.scss'
})
export class LanguageSwitcherComponent {
  private languageService = inject(LanguageService);

  get current(): Language {
    return this.languageService.getLanguage();
  }

  setLanguage(lang: Language): void {
    this.languageService.setLanguage(lang);
  }
}