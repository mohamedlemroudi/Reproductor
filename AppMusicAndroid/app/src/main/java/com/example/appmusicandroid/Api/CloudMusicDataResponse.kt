package com.example.appmusicandroid.Api

import com.google.gson.annotations.SerializedName

data class CloudMusicDataResponse(
    @SerializedName("\$values") val musicList: List<MusicItemResponse>
)

data class MusicItemResponse(
    @SerializedName("\$id") val id: String,
    @SerializedName("uid") val uid: String,
    @SerializedName("title") val title: String,
    @SerializedName("language") val language: String?,
    @SerializedName("duration") val duration: String?,
    @SerializedName("versionOriginalId") val versionOriginalId: String?,
    @SerializedName("originalSong") val originalSong: OriginalSong?,
    @SerializedName("playObj") val playObj: String?,
    @SerializedName("extensions") val extensions: String?,
    @SerializedName("playlists") val playlists: String?
)

data class OriginalSong(
    @SerializedName("\$id") val id: String,
    @SerializedName("uid") val uid: String,
    @SerializedName("title") val title: String,
    @SerializedName("language") val language: String?,
    @SerializedName("duration") val duration: String?,
    @SerializedName("versionOriginalId") val versionOriginalId: String?,
    @SerializedName("originalSong") val originalSong: OriginalSong?,
    @SerializedName("playObj") val playObj: String?,
    @SerializedName("songs") val songs: Any?,  // Cambiado a un tipo genérico para aceptar objeto o lista
    @SerializedName("extensions") val extensions: String?,
    @SerializedName("playlists") val playlists: String?
)

data class Song(
    @SerializedName("\$ref") val ref: String?  // Ajustado para que pueda ser nulo
)

