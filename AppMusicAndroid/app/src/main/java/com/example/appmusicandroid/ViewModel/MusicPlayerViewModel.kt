package com.example.appmusicandroid.ViewModel

import android.content.Context
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.example.appmusicandroid.Model.MusicModel
import com.example.appmusicandroid.Music.IFindMusic

class MusicPlayerViewModel : ViewModel() {

    val musicList = MutableLiveData<ArrayList<MusicModel>>()

    fun getMusicFile(iFindMusic: IFindMusic, context: Context){
        musicList.value = iFindMusic.getMusicFile(context)
    }
}