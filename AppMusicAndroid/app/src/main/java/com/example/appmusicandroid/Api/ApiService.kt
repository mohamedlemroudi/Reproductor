package com.example.appmusicandroid.Api

import com.example.appmusicandroid.Model.Song
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Path

interface ApiService {
    @GET("api/song")
    suspend fun getSongs(): Response<CloudMusicDataResponse>

    @GET("api/song/{uid}")
    suspend fun getSongInformation(@Path("uid") songUid:String): Response<MusicItemResponse>
}