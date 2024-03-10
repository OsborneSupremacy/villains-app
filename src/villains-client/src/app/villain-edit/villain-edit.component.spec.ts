import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VillainEditComponent } from './villain-edit.component';

describe('VillainEditComponent', () => {
  let component: VillainEditComponent;
  let fixture: ComponentFixture<VillainEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VillainEditComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VillainEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
