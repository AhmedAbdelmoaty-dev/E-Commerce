import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Pagination } from '../../Shared/models/pagination';
import { Product } from '../../Shared/models/product';
import { ShopParams } from '../../Shared/models/shopParams';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  private  http = inject(HttpClient)
  private Url=environment.apiUrl
  types:string[]=[]
  brands:string[]=[]
  
  getProducts(shopParams:ShopParams){
    let params=new HttpParams()
    if(shopParams.brands&&shopParams.brands.length>0){
      params=params.append('brands',shopParams.brands.join(','))
    }
    if(shopParams.types&&shopParams.types.length>0){
      params=params.append('types',shopParams.types.join(','))
    }
    if(shopParams.sort){
      params=params.append('sort',shopParams.sort)
    }
    if(shopParams.search){
      params=params.append('search',shopParams.search)
    }
    params=params.append('pageSize',shopParams.pageSize)
    params=params.append('pageIndex',shopParams.pageNumber)
    
    console.log(params)
   return this.http.get<Pagination<Product>>(this.Url+"products",{params,withCredentials:true})
  }
  getTypes(){
    if(this.types.length > 0) return
    this.http.get<string[]>(this.Url+"products/types").subscribe(response=>{
      console.log(response)
      this.types=response
    }
    )
  }
  getBrands(){
    if(this.brands.length>0) return
    this.http.get<string[]>(this.Url+"products/brands").subscribe(response=>{
      console.log(response)
      this.brands=response
    }
    )
  }
  getProductById(id:number){
    return this.http.get<Product>(this.Url+'products/'+id)
  }
}
