import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateAlertModalComponent } from './create-alert-modal.component';

describe('CreateAlertModalComponent', () => {
  let component: CreateAlertModalComponent;
  let fixture: ComponentFixture<CreateAlertModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateAlertModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateAlertModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
