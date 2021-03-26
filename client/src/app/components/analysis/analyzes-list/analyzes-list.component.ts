import { Component, OnInit } from '@angular/core';

interface AnalysisObject {
  ID: number;
  fileName: string;
  status: string;
  raports?: string;
}


@Component({
  selector: 'app-analyzes-list',
  templateUrl: './analyzes-list.component.html',
  styleUrls: ['./analyzes-list.component.scss']
})
export class AnalyzesListComponent implements OnInit {
  analysisData: AnalysisObject[] = []

  constructor() { }

  ngOnInit(): void {
    this.getMockupData();
  }

  private getMockupData() {
    const item1 = {
      ID: 1,
      fileName: 'example_filename.py',
      status: 'finished',
    } as AnalysisObject
    const item2 = {
      ID: 2,
      fileName: 'program.py',
      status: 'waiting',
    } as AnalysisObject
    const item3 = {
      ID: 3,
      fileName: 'I_am_not_cheating.py',
      status: 'canceled',
    } as AnalysisObject
    const item4 = {
      ID: 4,
      fileName: 'dont_click_me.py',
      status: 'ready',
    } as AnalysisObject
    this.analysisData.push(item1, item2, item3, item4);
  }

}
