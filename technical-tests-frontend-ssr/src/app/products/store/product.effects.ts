import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as ProductActions from './product.actions';
import { ProductService } from '../product.service';
import { catchError, map, of, switchMap } from 'rxjs';

@Injectable()
export class ProductsEffects {
  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProductActions.loadProducts),
      switchMap(() =>
        this.service.getAll().pipe(
          map((products) => ProductActions.loadProductsSuccess({ products })),
          catchError((err) =>
            of(ProductActions.loadProductsFailure({ error: err?.message ?? 'Unknown error' }))
          )
        )
      )
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProductActions.createProduct),
      switchMap(({ product }) =>
        this.service.create(product).pipe(
          map((created) => ProductActions.createProductSuccess({ product: created })),
          catchError((err) =>
            of(ProductActions.createProductFailure({ error: err?.message ?? 'Unknown error' }))
          )
        )
      )
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProductActions.updateProduct),
      switchMap(({ update }) =>
        this.service.update(String(update.id), update.changes).pipe(
          map((updated) =>
            ProductActions.updateProductSuccess({ update: { id: updated.id, changes: updated } })
          ),
          catchError((err) =>
            of(ProductActions.updateProductFailure({ error: err?.message ?? 'Unknown error' }))
          )
        )
      )
    )
  );

  delete$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProductActions.deleteProduct),
      switchMap(({ id }) =>
        this.service.delete(id).pipe(
          map((deletedId) => ProductActions.deleteProductSuccess({ id: deletedId })),
          catchError((err) =>
            of(ProductActions.deleteProductFailure({ error: err?.message ?? 'Unknown error' }))
          )
        )
      )
    )
  );

  constructor(private actions$: Actions, private service: ProductService) {}
}
