import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'category',
  styleUrls: ['./category.component.css'],
  templateUrl: './category.component.html'
})
export class CategoryComponent implements OnInit {

@Input() public category: any;

public Title: string;
public Description: string;
public ImgUrl: string;

public ngOnInit() {
this.Title = this.category.productFor + ' ' + this.category.productType;
this.Description = this.category.description;
this.ImgUrl = this.category.minioObject_URL;
}
}
