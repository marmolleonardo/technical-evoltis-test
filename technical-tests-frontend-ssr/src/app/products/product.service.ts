import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Product } from './store/product.models';

// Backend ASP.NET Core: ProductsController -> route "api/products"
// Development HTTPS endpoint to avoid CORS issues
const BASE_API = '/api';

interface ProductReadDto {
  id: number;
  name: string;
  price: number;
  createdAt: string; // ISO
  updatedAt?: string | null; // ISO
}

interface ProductCreateDto {
  name: string;
  price: number;
}

@Injectable({ providedIn: 'root' })
export class ProductService {
  constructor(private http: HttpClient) {}

  getAll(): Observable<Product[]> {
    return this.http
      .get<ProductReadDto[]>(`${BASE_API}/products`)
      .pipe(map((list) => list.map((dto) => this.fromReadDto(dto))));
  }

  create(product: Product): Observable<Product> {
    const body: ProductCreateDto = { name: product.name, price: product.price };
    return this.http
      .post<ProductReadDto>(`${BASE_API}/products`, body)
      .pipe(map((dto) => this.fromReadDto(dto)));
  }

  update(id: string, changes: Partial<Product>): Observable<Product> {
    const nid = Number(id);
    const body: ProductCreateDto = { name: changes.name as string, price: changes.price as number };
    return this.http
      .put<ProductReadDto>(`${BASE_API}/products/${nid}`, body)
      .pipe(map((dto) => this.fromReadDto(dto)));
  }

  delete(id: string): Observable<string> {
    const nid = Number(id);
    return this.http
      .delete<void>(`${BASE_API}/products/${nid}`, { observe: 'response' })
      .pipe(map(() => id));
  }

  // Helpers
  private fromReadDto(dto: ProductReadDto): Product {
    return {
      id: String(dto.id),
      name: dto.name,
      price: dto.price,
      createdAt: dto.createdAt,
      // Mapear updatedAt (backend) -> UpdatedAt (front)
      UpdatedAt: dto.updatedAt ?? dto.createdAt
    } as Product;
  }
}
