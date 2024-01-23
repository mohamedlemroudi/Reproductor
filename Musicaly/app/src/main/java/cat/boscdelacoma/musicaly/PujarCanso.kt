package cat.boscdelacoma.musicaly

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.ImageView

class PujarCanso : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_pujar_canso)

        val homeLayout: ImageView = findViewById(R.id.Home)
        val playlistLayout: ImageView = findViewById(R.id.Playlist)
        val album: ImageView = findViewById(R.id.Albums)

        homeLayout.setOnClickListener {
            val intent = Intent(this@PujarCanso, MainActivity::class.java)
            startActivity(intent)
        }
        playlistLayout.setOnClickListener {
            val intent = Intent(this@PujarCanso, Playlist::class.java)
            startActivity(intent)
        }
        album.setOnClickListener {
            val intent = Intent(this@PujarCanso, Album::class.java)
            startActivity(intent)
        }
    }
}