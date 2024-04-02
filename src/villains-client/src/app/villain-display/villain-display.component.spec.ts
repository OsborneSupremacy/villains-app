import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VillainDisplayComponent } from './villain-display.component';

describe('VillainDisplayComponent', () => {
  let component: VillainDisplayComponent;
  let fixture: ComponentFixture<VillainDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VillainDisplayComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VillainDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
