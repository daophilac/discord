package com.peanut.discord;

import android.app.Dialog;
import android.content.Context;
import android.os.Bundle;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.DialogFragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;

import com.peanut.discord.equipment.HubManager;
import com.peanut.discord.equipment.Inventory;
import com.peanut.discord.tools.JsonBuilder;

public class CreateChannelDialogFragment extends DialogFragment {
    private EditText editTextChannelName;
    private Button buttonOk;
    private JsonBuilder jsonBuilder;

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        jsonBuilder = new JsonBuilder();
    }

    @NonNull
    @Override
    public Dialog onCreateDialog(@Nullable Bundle savedInstanceState) {
        Dialog dialog = super.onCreateDialog(savedInstanceState);
        dialog.setTitle("Create channel");
        return dialog;
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = MainActivity.themeInflater.inflate(R.layout.dialog_create_channel, container, false);
        editTextChannelName = view.findViewById(R.id.edit_text_channel_name);
        buttonOk = view.findViewById(R.id.button_ok);
        buttonOk.setOnClickListener(v -> {
            if(!editTextChannelName.getText().toString().equals("")){
                String json = jsonBuilder.buildChannelJson(editTextChannelName.getText().toString(), Inventory.currentServer.getServerId());
                HubManager.createChannel(Inventory.currentServer.getServerId(), json);
                dismiss();
            }
        });
        return view;
    }
}
