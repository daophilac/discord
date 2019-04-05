package com.daophilac.discord;

import android.content.ContextWrapper;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.daophilac.discord.filedealer.InternalFileReader;
import com.daophilac.discord.filedealer.InternalFileWriter;
import com.daophilac.discord.resources.Route;
import com.daophilac.discord.tools.APICaller;
import com.daophilac.discord.tools.JsonBuilder;

import java.io.File;

public class LoginActivity extends AppCompatActivity {
    public Handler backgroundHandler;
    private Thread threadBackground;
    private APICaller apiCaller;
    private JsonBuilder jsonBuilder;
    private EditText editTextEmail;
    private EditText editTextPassword;
    private Button buttonLogin;
    private String baseURL;
    private boolean isLoggedIn;
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if(!checkLogin()){
            setContentView(R.layout.activity_login);
            initializeGlobalVariable();
        }
    }
    private void initializeGlobalVariable(){
        this.isLoggedIn = false;
        this.backgroundHandler = new Handler(Looper.getMainLooper()){
            @Override
            public void handleMessage(Message msg) {
                super.handleMessage(msg);
                handleJSON(msg.obj.toString());
            }
        };
        this.editTextEmail = findViewById(R.id.editTextEmail);
        this.editTextPassword = findViewById(R.id.editTextPassword);
        this.buttonLogin = findViewById(R.id.buttonLogin);
        this.buttonLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                login();
            }
        });
        this.baseURL = Route.protocol + "://" + Route.serverIP + "/" + Route.serverName;
        this.apiCaller = new APICaller(this.backgroundHandler, "POST");
        this.jsonBuilder = new JsonBuilder();
    }
    private boolean checkLogin(){
        ContextWrapper contextWrapper = new ContextWrapper(this);
        File file = new File(contextWrapper.getFilesDir() + "/" + getString(R.string.account_internal_directory) + "/" + getString(R.string.account_internal_file));
        if(file.exists()){
            this.isLoggedIn = true;
            autoLogin();
            return true;
        }
        return false;
    }
    private void autoLogin(){
        InternalFileReader internalFileReader = new InternalFileReader(getString(R.string.account_internal_directory), getString(R.string.account_internal_file), this);
        String email = internalFileReader.readLine();
        String password = internalFileReader.readLine();
        this.jsonBuilder = new JsonBuilder();
        String json = this.jsonBuilder.buildLoginJSON(email, password);
        this.backgroundHandler = new Handler(Looper.getMainLooper()){
            @Override
            public void handleMessage(Message msg) {
                super.handleMessage(msg);
                handleJSON(msg.obj.toString());
            }
        };
        this.baseURL = Route.protocol + "://" + Route.serverIP + "/" + Route.serverName;
        String requestURL = this.baseURL.concat(Route.urlLogin);
        this.apiCaller = new APICaller(this.backgroundHandler, "POST");
        this.apiCaller.setRequestURL(requestURL);
        this.apiCaller.setJSON(json);
        this.threadBackground = new Thread(this.apiCaller);
        this.threadBackground.start();
    }
    private void login(){
        String email = this.editTextEmail.getText().toString();
        String password = this.editTextPassword.getText().toString();
        String json = this.jsonBuilder.buildLoginJSON(email, password);
//        HashMap<String, String> parameters = new HashMap<String, String>();
//        parameters.put("Email", email);
//        parameters.put("Password", password);
        String requestURL = this.baseURL.concat(Route.urlLogin);
        this.apiCaller.setRequestURL(requestURL);
        this.apiCaller.setJSON(json);
        this.threadBackground = new Thread(this.apiCaller);
        this.threadBackground.start();
    }
    private void writeUserAccount(){
        String email = this.editTextEmail.getText().toString();
        String password = this.editTextPassword.getText().toString();
        InternalFileWriter internalFileWriter = new InternalFileWriter(getString(R.string.account_internal_directory), getString(R.string.account_internal_file), this);
        internalFileWriter.write(email, false);
        internalFileWriter.write("\n", true);
        internalFileWriter.write(password, true);
    }
    private void handleJSON(String incomingJSON){
        if(incomingJSON.equals("")){
            Toast.makeText(this, "Not exist.", Toast.LENGTH_LONG).show();
        }
        else{
            if(!this.isLoggedIn){
                writeUserAccount();
            }
            Intent intent = new Intent(this, MainActivity.class);
            intent.putExtra("jsonUser", incomingJSON);
            startActivity(intent);
            finish();
        }
    }
}
