package com.example.appmusicandroid.Music

import android.content.Context
import com.example.appmusicandroid.Model.MusicModel

interface IFindMusic {
    fun getMusicFile(context: Context) : ArrayList<MusicModel>?
}