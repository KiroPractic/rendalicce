import {Component, EventEmitter, Input, Output} from '@angular/core';
import {FormsModule} from "@angular/forms";
import {NgClass, NgForOf} from "@angular/common";

@Component({
  selector: 'app-tag-input',
  standalone: true,
  imports: [
    FormsModule,
    NgForOf,
    NgClass
  ],
  templateUrl: './tag-input.component.html',
  styleUrl: './tag-input.component.scss'
})
export class TagInputComponent {
  @Input() tags: string[] = [];
  @Output() tagsChange = new EventEmitter<string[]>();

  newTag: string = '';
  tagColors: string[] = ['bg-red-200', 'bg-green-200', 'bg-blue-200', 'bg-yellow-200', 'bg-purple-200', 'bg-pink-200'];

  addTag(): void {
    if (this.newTag.trim()) {
      this.tags.push(this.newTag.trim());
      this.tagsChange.emit(this.tags);
      this.newTag = '';
    }
  }

  removeTag(index: number): void {
    this.tags.splice(index, 1);
    this.tagsChange.emit(this.tags);
  }

  getTagColor(index: number): string {
    return this.tagColors[index % this.tagColors.length];
  }
}
