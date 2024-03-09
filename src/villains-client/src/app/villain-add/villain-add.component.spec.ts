import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VillainAddComponent } from './villain-add.component';

describe('VillainAddComponent', () => {
  let component: VillainAddComponent;
  let fixture: ComponentFixture<VillainAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VillainAddComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VillainAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
