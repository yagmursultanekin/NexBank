import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Branch } from '../models/branch.model';

@Injectable({
  providedIn: 'root'
})
export class BranchService {
  private apiUrl = 'https://localhost:7100/api/branch';

  constructor(private http: HttpClient) {}

  getNearest(lat: number, lng: number, distanceKm: number = 1): Observable<Branch[]> {
    return this.http.get<Branch[]>(
      `${this.apiUrl}/nearest?lat=${lat}&lng=${lng}&distance=${distanceKm}`
    );
  }
}