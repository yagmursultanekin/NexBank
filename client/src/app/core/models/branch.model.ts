export interface Branch {
  id: number;
  name: string;
  type: string; // "ATM" | "Şube"
  address: string;
  phone: string;
  latitude: number;
  longitude: number;
  distanceKm: number;
}