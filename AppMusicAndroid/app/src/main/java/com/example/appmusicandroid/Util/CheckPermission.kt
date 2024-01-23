package com.example.appmusicandroid.Util

import android.content.Context
import android.widget.Toast
import androidx.lifecycle.MutableLiveData
import com.example.appmusicandroid.Model.MusicModel
import com.example.appmusicandroid.Music.IFindMusic
import com.karumi.dexter.Dexter
import com.karumi.dexter.PermissionToken
import com.karumi.dexter.listener.PermissionDeniedResponse
import com.karumi.dexter.listener.PermissionGrantedResponse
import com.karumi.dexter.listener.PermissionRequest
import com.karumi.dexter.listener.single.PermissionListener

class CheckPermission(private val findMusic: IFindMusic) {

    val musicList = MutableLiveData<ArrayList<MusicModel>>()

    fun checkPermissions(context: Context){
        Dexter.withContext(context)
            .withPermission(android.Manifest.permission.READ_MEDIA_AUDIO)
            .withListener(object : PermissionListener {
                override fun onPermissionGranted(p0: PermissionGrantedResponse?) {
                    musicList.value = findMusic.getMusicFile(context)
                }
                override fun onPermissionDenied(p0: PermissionDeniedResponse?) {
                    Toast.makeText(context, "Permission is required", Toast.LENGTH_SHORT).show()
                }
                override fun onPermissionRationaleShouldBeShown(
                    p0: PermissionRequest?,
                    p1: PermissionToken?
                ) {
                    p1?.continuePermissionRequest()
                }
            }).check()
    }
}