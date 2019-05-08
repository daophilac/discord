package com.peanut.discord;

import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.view.Gravity;
import android.widget.Button;
import android.widget.EditText;

import com.peanut.discord.customview.BackButton;
import com.peanut.discord.resources.Route;
import com.peanut.discord.tools.APICaller;
import com.peanut.discord.worker.SingleWorker;

public class CreateServerActivity extends AppCompatActivity {
    private int currentUserId;
    private APICaller apiCaller;
    private Handler handler;
    private SingleWorker singleWorker;
    private BackButton backButton;
    private Button buttonCreate;
    private EditText editTextServerName;
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setTheme(MainActivity.themeId);
        setContentView(R.layout.activity_create_server);
        currentUserId = getIntent().getIntExtra("currentUserId", -1);
        singleWorker = new SingleWorker(MainActivity.LOG_TAG);
        apiCaller = new APICaller();

        backButton = findViewById(R.id.back_button);
        buttonCreate = findViewById(R.id.button_create);
        editTextServerName = findViewById(R.id.edit_text_server_name);


        backButton.setOnClickListener(v -> finish());
        buttonCreate.setOnClickListener(v -> {
            if(!editTextServerName.getText().toString().equals("")){
                String json = MainActivity.jsonBuilder.buildServerJson(currentUserId, editTextServerName.getText().toString());
                apiCaller.setProperties(handler, APICaller.RequestMethod.POST, Route.urlPostServer, json);
                singleWorker.execute(apiCaller);
            }
        });
        handler = new Handler(Looper.getMainLooper()){
            @Override
            public void handleMessage(Message msg) {
                Intent intent = new Intent(CreateServerActivity.this, MainActivity.class);
                intent.putExtra("command", MainActivity.IntentCommand.CREATE_SERVER);
                intent.putExtra("jsonServer", (String)msg.obj);
                startActivity(intent);
                finish();
            }
        };
    }
}