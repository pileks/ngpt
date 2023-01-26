import { Pipe, PipeTransform, ChangeDetectorRef } from '@angular/core';
import * as moment from 'moment';
import { timer, Subscription } from 'rxjs';

const timerRefreshIntervalMiliseconds = 1000;

@Pipe({
  name: 'augurMinutesRemaining',
  pure: false
})
export class AugurMinutesRemainingPipe implements PipeTransform {

  constructor(private changeDetectorRef: ChangeDetectorRef) {
  }

  private currentDate: Date;
  private lastDate: Date;
  private timerSubscription: Subscription;

  transform(value: Date): string {
    this.removeTimerIfDateChanged();
    this.createTimer();

    this.lastDate = this.currentDate;
    this.currentDate = value;

    return this.formatDuration(this.getDurationUntil(value));
  }

  private createTimer() {
    if (!this.timerSubscription || this.timerSubscription.closed) {
      const timerObs = timer(0, timerRefreshIntervalMiliseconds);
      this.timerSubscription = timerObs.subscribe(t => {
        this.changeDetectorRef.markForCheck();
      });
    }
  }

  private removeTimerIfDateChanged() {
    if (this.timerSubscription && this.currentDate !== this.lastDate) {
      this.timerSubscription.unsubscribe();
    }
  }

  private formatDuration(duration: moment.Duration): string {
    var secondsRemaining = Math.round(duration.asSeconds());
    var minutes = Math.floor(secondsRemaining / 60);
    var seconds = Math.round(secondsRemaining - minutes * 60);
    var minutesString = minutes.toString();
    var secondsString = seconds.toString();
    if (minutes < 10) { minutesString = '0' + minutes; }
    if (seconds < 10) { secondsString = '0' + seconds; }

    if (secondsRemaining >= 0) {
      return minutesString + ':' + secondsString;
    } else {
      return '00:00';
    }
  }

  private getDurationUntil(value): moment.Duration {
    var start_date = moment();
    var end_date = moment(value);

    return moment.duration(end_date.diff(start_date));
  }
}