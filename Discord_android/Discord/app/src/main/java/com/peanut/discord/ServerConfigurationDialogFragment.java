package com.peanut.discord;

import android.app.Dialog;
import android.content.Context;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.DialogFragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.TextView;

import com.peanut.discord.interfaces.ServerConfigurationListener;

public class ServerConfigurationDialogFragment extends DialogFragment {
    private ServerConfigurationListener serverConfigurationListener;
    private TextView textViewLeaveServer;
    @NonNull
    @Override
    public Dialog onCreateDialog(@Nullable Bundle savedInstanceState) {
        Dialog dialog = super.onCreateDialog(savedInstanceState);
        dialog.getWindow().requestFeature(Window.FEATURE_NO_TITLE);
        return dialog;
    }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        serverConfigurationListener = (ServerConfigurationListener)context;
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = MainActivity.themeInflater.inflate(R.layout.dialog_server_configuration, container, false);
        textViewLeaveServer = view.findViewById(R.id.text_view_leave_server);

        textViewLeaveServer.setOnClickListener(v -> {
            dismiss();
            serverConfigurationListener.onLeaveServer();
        });
        return view;
    }
}
