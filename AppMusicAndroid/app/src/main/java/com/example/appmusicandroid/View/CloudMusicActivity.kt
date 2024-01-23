package com.example.appmusicandroid.View

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import android.widget.Toast
import androidx.appcompat.widget.SearchView
import androidx.core.view.isVisible
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.appmusicandroid.Adaper.CloudMusicAdapter
import com.example.appmusicandroid.Adaper.CurrentAdapter
import com.example.appmusicandroid.Adaper.IOnItemClickListener
import com.example.appmusicandroid.Api.ApiService
import com.example.appmusicandroid.Api.MusicItemResponse
import com.example.appmusicandroid.Model.MusicModel
import com.example.appmusicandroid.Model.Song
import com.example.appmusicandroid.R
import com.example.appmusicandroid.View.PlayCloudMusicActivity.Companion.EXTRA_UID
import com.example.appmusicandroid.databinding.ActivityCloudMusicBinding
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.util.ArrayList
import java.util.Locale

class CloudMusicActivity : AppCompatActivity() {

    private lateinit var binding: ActivityCloudMusicBinding
    private lateinit var retrofit: Retrofit
    private lateinit var adapter: CloudMusicAdapter
    private lateinit var musicList : List<MusicItemResponse>
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityCloudMusicBinding.inflate(layoutInflater)
        setContentView(binding.root)
        retrofit = getRetrofit()
        initUI()
        initListener()
    }

    private fun initUI() {
        getMusicCloud()
        search()
        adapter = CloudMusicAdapter {
            navigateToDetail(it)
        }
        binding.rvCloudMusic.setHasFixedSize(true)
        binding.rvCloudMusic.layoutManager = LinearLayoutManager(this)
        binding.rvCloudMusic.adapter = adapter
    }

    private fun initListener() {
        binding.Home.setOnClickListener {
            val intent = Intent(this@CloudMusicActivity, CurrentActivity::class.java)
            startActivity(intent)
        }
    }

    private fun getMusicCloud() {
        binding.progressBar.isVisible = true
        CoroutineScope(Dispatchers.IO).launch {
            try {
                val myResponse = retrofit.create(ApiService::class.java).getSongs()
                if (myResponse.isSuccessful) {
                    val response = myResponse.body()
                    if(response != null) {
                        runOnUiThread {
                            adapter.updateList(response.musicList)
                            binding.progressBar.isVisible = false
                            musicList = response.musicList
                        }
                    }

                } else {
                    // Manejar la respuesta no exitosa
                    Log.e("ApiService", "Error1: ${myResponse.code()}")
                }
            } catch (e: Exception) {
                // Manejar excepciones
                Log.e("ApiService", "Error2: ${e.message}", e)
            }
        }
    }

    private fun search(){
        binding.searchView.setOnQueryTextListener(object : SearchView.OnQueryTextListener,
            android.widget.SearchView.OnQueryTextListener {
            override fun onQueryTextSubmit(query: String?): Boolean {
                return true
            }
            override fun onQueryTextChange(newText: String?): Boolean {
                filter1(newText!!)
                return true
            }
        })
    }

    private fun filter1(text : String){
        val filterList = ArrayList<MusicItemResponse>()

        musicList.forEach {
            if (it.title.toLowerCase(Locale.ROOT).contains(text.toLowerCase(Locale.ROOT))){
                filterList.add(it)
            }
        }

        if (filterList.isEmpty()){
            Toast.makeText(this, "Music is not found", Toast.LENGTH_SHORT).show()
        }else{
            initRecycler(filterList)
        }
    }

    private fun initRecycler(musicModelList : ArrayList<MusicItemResponse>){
        adapter = CloudMusicAdapter {
            navigateToDetail(it)
        }
        binding.rvCloudMusic.setHasFixedSize(true)
        binding.rvCloudMusic.layoutManager = LinearLayoutManager(this)
        binding.rvCloudMusic.adapter = adapter
        adapter.setFilterList(musicModelList)

    }

    private fun getRetrofit(): Retrofit {
        return Retrofit
            .Builder()
            .baseUrl("http://localhost:5095/")
            .addConverterFactory(GsonConverterFactory.create())
            .build()
    }

    private fun navigateToDetail(id:String) {
        val intent = Intent(this, PlayCloudMusicActivity::class.java)
        intent.putExtra(EXTRA_UID, id)
        startActivity(intent)
    }
}