package com.example.appmusicandroid.ViewModel

import android.content.Context
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.example.appmusicandroid.Model.MusicModel
import com.example.appmusicandroid.Music.IFindMusic
import com.example.appmusicandroid.Util.CheckPermission

class CurrentViewModel() : ViewModel() {

    val musicList = MutableLiveData<ArrayList<MusicModel>>()

    private lateinit var checkPermission: CheckPermission

    fun checkPermission(findMusic: IFindMusic, context: Context){
        checkPermission = CheckPermission(findMusic)
        checkPermission.checkPermissions(context)
    }

    fun getMusic(lifecycleOwner: LifecycleOwner){
        checkPermission.musicList.observe(lifecycleOwner) {
            musicList.value = it
        }
    }
}