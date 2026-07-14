import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LanguageService } from './core/services/language.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  protected readonly title = signal('client');

  private languageService = inject(LanguageService);

  ngOnInit(): void {
    // Kullanıcının kayıtlı dil tercihini geri yükle (yoksa Türkçe)
    this.languageService.init();
  }
}