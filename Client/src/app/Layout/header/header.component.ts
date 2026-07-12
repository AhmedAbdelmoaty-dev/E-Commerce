import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatBadge } from '@angular/material/badge';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { MatProgressBar } from '@angular/material/progress-bar';
import { BusyService } from '../../Core/Services/busy.service';
import { CartService } from '../../Core/Services/cart-service';
import { AccountService } from '../../Core/Services/account.service';
import { MatMenuItem, MatMenuTrigger } from '@angular/material/menu';
import { MatDivider } from '@angular/material/divider';
@Component({
  selector: 'app-header',
  imports: [
    MatButton,
    MatIcon,
    MatBadge,
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    MatProgressBar,
    MatMenuTrigger,
    MatDivider,
    MatMenuItem
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent {
  busyService = inject(BusyService);
  cartService = inject(CartService);
  accountService=inject(AccountService)
  private router=inject(Router)

  logout(){
    this.accountService.logout().subscribe({
      next:()=>{
        this.accountService.currentUser.set(null),
        console.log(this.accountService.currentUser());
        this.router.navigateByUrl("/")
      }
    })
  }
  //test ci cd
}
