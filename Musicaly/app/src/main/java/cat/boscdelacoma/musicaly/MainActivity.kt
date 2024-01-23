package cat.boscdelacoma.musicaly

import android.content.Intent
import android.os.Bundle
import android.widget.LinearLayout
import android.widget.ImageView
import androidx.appcompat.app.AppCompatActivity

class MainActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        val canco1Layout: LinearLayout = findViewById(R.id.Canco1)
        val playlistLayout: ImageView = findViewById(R.id.Playlist)
        val albumLayout: ImageView = findViewById(R.id.Albums)
        val subircancionLayout: ImageView = findViewById(R.id.CrearCanco)

        canco1Layout.setOnClickListener {
            val intent = Intent(this@MainActivity, MusicPlayer::class.java)
            startActivity(intent)
        }
        playlistLayout.setOnClickListener {
            val intent = Intent(this@MainActivity, Playlist::class.java)
            startActivity(intent)
        }
        albumLayout.setOnClickListener {
            val intent = Intent(this@MainActivity, Album::class.java)
            startActivity(intent)
        }
        subircancionLayout.setOnClickListener {
            val intent = Intent(this@MainActivity, PujarCanso::class.java)
            startActivity(intent)
        }
    }
}
