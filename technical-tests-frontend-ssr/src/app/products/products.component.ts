import { Component, OnInit, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { Product, datetime } from './store/product.models';
import * as ProductActions from './store/product.actions';
import * as ProductSelectors from './store/product.selectors';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit {
  private store = inject(Store);
  private confirm = inject(ConfirmationService);

  products$: Observable<Product[]> = this.store.select(ProductSelectors.selectAllProducts);
  loading$: Observable<boolean> = this.store.select(ProductSelectors.selectProductsLoading);
  error$: Observable<string | null> = this.store.select(ProductSelectors.selectProductsError);

  dialogVisible = false;
  isEdit = false;
  editingId: string | null = null;

  // Reactive form with typed FormControls
  form: FormGroup<{
    name: FormControl<string>;
    price: FormControl<number | null>;
  }> = new FormGroup({
    name: new FormControl<string>('', { nonNullable: true, validators: [Validators.required, Validators.maxLength(100)] }),
    price: new FormControl<number | null>(null, { validators: [Validators.required, Validators.min(0)] })
  });

  ngOnInit(): void {
    // 1) El componente dispara la acción inicial
    this.store.dispatch(ProductActions.loadProducts());
  }

  // Toolbar actions
  openCreate(): void {
    this.isEdit = false;
    this.editingId = null;
    this.form.reset({ name: '', price: null });
    this.dialogVisible = true;
  }

  openEdit(p: Product): void {
    this.isEdit = true;
    this.editingId = p.id;
    this.form.reset({ name: p.name, price: p.price });
    this.dialogVisible = true;
  }

  private markAllAsTouched(): void {
    this.form.markAllAsTouched();
    this.form.updateValueAndValidity();
  }

  save(): void {
    if (this.form.invalid) { this.markAllAsTouched(); return; }
    const { name, price } = this.form.getRawValue();

    if (this.isEdit && this.editingId) {
      this.store.dispatch(
        ProductActions.updateProduct({ update: { id: this.editingId, changes: { name, price: price as number } as any } })
      );
    } else {
      const now: datetime = new Date().toISOString();
      const product: Product = { id: '', name, price: price as number, createdAt: now, UpdatedAt: now };
      this.store.dispatch(ProductActions.createProduct({ product }));
    }

    this.dialogVisible = false;
  }

  confirmDelete(p: Product): void {
    this.confirm.confirm({
      message: `¿Eliminar "${p.name}"?`,
      header: 'Confirmar eliminación',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Eliminar',
      rejectLabel: 'Cancelar',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => this.store.dispatch(ProductActions.deleteProduct({ id: p.id }))
    });
  }
}
