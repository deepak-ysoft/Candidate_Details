import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CandidateDetailsComponent } from "../components/candidate-details/candidate-details.component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CandidateDetailsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'CandidateDetails';
}
