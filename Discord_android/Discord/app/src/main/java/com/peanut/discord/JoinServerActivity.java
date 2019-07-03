package com.peanut.discord;

import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.peanut.discord.customview.BackButton;
import com.peanut.discord.resources.Route;
import com.peanut.discord.tools.APICaller;
import com.peanut.discord.worker.SingleWorker;

public class JoinServerActivity extends AppCompatActivity {
    private int currentUserId;
    private APICaller apiCaller;
    private Handler handler;
    private SingleWorker singleWorker;
    private BackButton backButton;
    private Button buttonJoin;
    private EditText editTextInstantInvite;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setTheme(MainActivity.themeId);
        setContentView(R.layout.activity_join_server);
        currentUserId = getIntent().getIntExtra("currentUserId", -1);
        apiCaller = new APICaller();
        singleWorker = new SingleWorker(MainActivity.LOG_TAG);
        backButton = findViewById(R.id.back_button);
        buttonJoin = findViewById(R.id.button_join);
        editTextInstantInvite = findViewById(R.id.edit_text_instant_invite);

        backButton.setOnClickListener(v -> finish());
        buttonJoin.setOnClickListener(v -> {
            if(!editTextInstantInvite.getText().toString().equals("")){
                apiCaller.setProperties(handler, APICaller.RequestMethod.GET, Route.buildGetServerByInstantInviteUrl(currentUserId, editTextInstantInvite.getText().toString()));
                singleWorker.execute(apiCaller);
            }
        });
        handler = new Handler(Looper.getMainLooper()){
            @Override
            public void handleMessage(Message msg) {
                if(msg.what == 1){
                    Intent intent = new Intent(JoinServerActivity.this, MainActivity.class);
                    intent.putExtra("command", MainActivity.IntentCommand.JOIN_SERVER);
                    intent.putExtra("jsonServer", (String)msg.obj);
                    startActivity(intent);
                    finish();
                }
                else{
                    Toast.makeText(getBaseContext(), getString(R.string.instant_invite_does_not_exist), Toast.LENGTH_LONG).show();
                }
            }
        };
    }
}