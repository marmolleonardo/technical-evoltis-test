import { createReducer, on } from '@ngrx/store';
import { EntityState, EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import * as ProductActions from './product.actions';
import { Product } from './product.models';

export const productsFeatureKey = 'products';

export interface ProductState extends EntityState<Product> {
  loading: boolean;
  error: string | null;
}

export const adapter: EntityAdapter<Product> = createEntityAdapter<Product>({
  selectId: (p) => p.id
});

export const initialState: ProductState = adapter.getInitialState({
  loading: false,
  error: null
});

const internalReducer = createReducer(
  initialState,
  on(ProductActions.loadProducts, (state) => ({ ...state, loading: true, error: null })),
  on(ProductActions.loadProductsSuccess, (state, { products }) =>
    adapter.setAll(products, { ...state, loading: false })
  ),
  on(ProductActions.loadProductsFailure, (state, { error }) => ({ ...state, loading: false, error })),

  on(ProductActions.createProductSuccess, (state, { product }) => adapter.addOne(product, state)),
  on(ProductActions.updateProductSuccess, (state, { update }) => adapter.updateOne(update, state)),
  on(ProductActions.deleteProductSuccess, (state, { id }) => adapter.removeOne(id, state))
);

export function productsReducer(state: ProductState | undefined, action: any): ProductState {
  return internalReducer(state, action);
}
